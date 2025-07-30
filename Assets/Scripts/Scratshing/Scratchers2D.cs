using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scratchers2D : MonoBehaviour
{
	[SerializeField]
	private List<Texture2D> _spellTextures;
	[SerializeField, Tooltip("Payload Texture (what to reveal)")]
	private Texture2D _underlyingTexture;
    [SerializeField, Tooltip("Gray obscuration covering texture")]
    private Texture2D _obscuringTexture;

    [SerializeField, Tooltip("What our finger scratches upon.")]
    public Collider _touchableCollider;

    [SerializeField, Tooltip("Where we display our result.")]
    public Renderer _finalResultRenderer;

	[Header( "The stage where stuff is filmed.")]
	// what will film each chunk of work as we scratch
	public Camera StageCamera;
	// what will display each chunk of work as we scratch
	public Renderer StageRenderer;
	public MeshFilter StageMeshFilter;
	public RenderTexture RT;

	public Material UnlitMaterial;

	[Header( "Coordinates in 1x1 UV space.")]
	public float MinScratchStreakWidth = 0.025f;
	public float MaxScratchStreakWidth = 0.035f;
	public float ScratchMarkSpacing = 0.030f;

	public Mesh BackgroundMesh;
	public GameObject MouseObject;

    [SerializeField]
    private LayerMask _scratchLayer;
    [SerializeField]
    private float _scratchSpeed = 1;
    [SerializeField]
    private GameEvent _scratchingDone;

	Material UnderlyingMaterial;

	Material ResultMaterial;
	Mesh workMesh;

	Camera mainCam;

    List<Vector3> _vertices = new List<Vector3>();
    List<Vector2> _uvs = new List<Vector2>();
    List<int> _tris = new List<int>();

	private int _scratchedSpots = 0;
	private int _maxScratchedSpots = 10;

	private Spells _currentSpell = 0;

	private Vector2 _scratchInput;

    private Vector3 _newPos = Vector3.zero;

    private PlayerInput _playerInput;

    private bool _initialized = false;



    public void initialize()
    {
        workMesh = new Mesh();
        workMesh.Clear();
        workMesh.RecalculateBounds();
        workMesh.RecalculateNormals();

        StageMeshFilter.mesh = workMesh;
        StageMeshFilter.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        mainCam = Camera.main;

        int randomSpell = Random.Range(0, 4);

        switch (randomSpell)
        {
            case 0:
                _currentSpell = Spells.Default;
                _underlyingTexture = _spellTextures[0];

                break;
            case 1:
                _currentSpell = Spells.WaterBall;
                _underlyingTexture = _spellTextures[1];
                break;
            case 2:
                _currentSpell = Spells.FireBall;
                _underlyingTexture = _spellTextures[2];
                break;
            case 3:
                _currentSpell = Spells.AcidBall;
                _underlyingTexture = _spellTextures[3];
                break;
        }

        UnderlyingMaterial = new Material(UnlitMaterial);
        UnderlyingMaterial.mainTexture = _underlyingTexture;

        ResultMaterial = new Material(UnlitMaterial);
        ResultMaterial.mainTexture = RT;
        _finalResultRenderer.material = ResultMaterial;

        _playerInput =  transform.parent.transform.parent.GetComponent<PlayerInput>();

        _initialized = true;

    }

	// to ensure we don't scratch until the obscuration has
	// had a chance to render properly.
	float startupTimer;
    const float startupDelay = 0.1f;

    void Update()
    {
        if (!_initialized) return;
        if (startupTimer > startupDelay)
        {
            // As soon as we're ready, wipe out the mesh utterly so that
            // we don't just give away the entire under surface immediately.
            //workMesh.Clear();
            StageMeshFilter.mesh = workMesh;

            StageRenderer.material = UnderlyingMaterial;
            if(_playerInput.actions["Move"].ReadValue<Vector2>() != Vector2.zero)
                UpdateScratching();
        }
		//MouseObject.transform.position = _touchableCollider.transform.position;
        startupTimer += Time.deltaTime;
    }

    void StrikeTriangle( Vector3 position1, Vector3 position2)
	{
		// streak between the two points given
		int triangleCount = 1 + (int)(Vector3.Distance( position1, position2) / ScratchMarkSpacing);

		// strike each series of triangles from position1 to position2
		for (int point = 0; point < triangleCount; point++)
		{
			float fraction = (point + 1.0f) / triangleCount;

			Vector3 position = Vector3.Lerp( position1, position2, fraction);

			// each one can be a slightly different size
			float size = Random.Range( MinScratchStreakWidth, MaxScratchStreakWidth);

			// and each one is differently-rotated
			float angle = Random.Range( 0.0f, 360.0f);

			// make the verts going around this particular triangle
			for (int i = 0; i < 3; i++)
			{
				Quaternion rotation = Quaternion.Euler( 0, 0, angle);

				Vector3 pointPosition = rotation * Vector3.right * size;

				// done first so count is correct (+0, +1, +2)
				_tris.Add( _vertices.Count);

				Vector3 finalVertexPosition = position + pointPosition;

				_vertices.Add( finalVertexPosition);

				// UV is same as vert, but offset half a unit
				_uvs.Add( finalVertexPosition + Vector3.one / 2);

				// rotate clockwise to match winding order of triangles
				angle -= 120;
			}
		}

		workMesh.vertices = _vertices.ToArray();
		workMesh.uv = _uvs.ToArray();
		workMesh.triangles = _tris.ToArray();

		workMesh.RecalculateBounds();
		workMesh.RecalculateNormals();

		StageMeshFilter.mesh = workMesh;
    }

	// We scratch from this position to the current position,
	// which mean swe need two consecutive good touches for
	// any rendering to happen.
	Vector3? PreviousPosition;

	void UpdateScratching()
	{
        Vector3? CurrentPosition = null;

        _scratchInput = _playerInput.actions["Move"].ReadValue<Vector2>();
        _newPos += new Vector3(_scratchInput.x, _scratchInput.y, 0) * _scratchSpeed * Time.deltaTime;
        _newPos.z = 0.5f;
        MouseObject.transform.localPosition = _newPos;

        RaycastHit hitInfo;

        if (Physics.Raycast(MouseObject.transform.position, MouseObject.transform.forward, out hitInfo, 100f, _scratchLayer))
        {
            Vector3 position = hitInfo.point;

            position = _touchableCollider.transform.InverseTransformPoint(position);

            CurrentPosition = position;

            if ((PreviousPosition != null) &&
                (CurrentPosition != null))
            {
                StrikeTriangle(
                    (Vector3)PreviousPosition,
                    (Vector3)CurrentPosition);
            }
        }

        PreviousPosition = CurrentPosition;
    }

	public void CheckForCompletion(Component sender, object obj)
	{
		if (sender.transform.parent.transform.parent.gameObject != gameObject) return;

		_scratchedSpots ++;

		if (_maxScratchedSpots > _scratchedSpots) return;

		_scratchedSpots = 0;

		RevealAllScratchArea();

        Debug.Log("Completed");
    }

    void RevealAllScratchArea()
    {
        float quadSize = 0.5f; // Total width/height will be 30 units
        List<Vector3> quadVerts = new List<Vector3>
    {
        new Vector3(-quadSize, -quadSize, 0f),
        new Vector3(quadSize, -quadSize, 0f),
        new Vector3(quadSize, quadSize, 0f),
        new Vector3(-quadSize, quadSize, 0f),
    };

        List<int> quadTris = new List<int> { 0, 1, 2, 2, 3, 0 };

        List<Vector2> quadUVs = new List<Vector2>
    {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),
    };

        _vertices = quadVerts;
        _tris = quadTris;
        _uvs = quadUVs;

        workMesh.Clear();
        workMesh.vertices = _vertices.ToArray();
        workMesh.triangles = _tris.ToArray();
        workMesh.uv = _uvs.ToArray();
        workMesh.RecalculateBounds();
        workMesh.RecalculateNormals();

        StageMeshFilter.mesh = workMesh;
        StageMeshFilter.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));

        _scratchingDone.Raise(this, _currentSpell);
    }
}
