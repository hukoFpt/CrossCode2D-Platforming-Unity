namespace CrossCode2D.Enemies
{
    public class JungleShockcat : Enemy
    {
        public static JungleShockcat Instance { get; private set; }

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected override void Start()
        {
            base.Start();
            Stats.InitializeStats(40, 2000, 2000, 189, 187, -0.2f, -0.2f, 1f, -0.4f);
        }

        protected override void Update()
        {
            base.Update();
        }

        void FixedUpdate()
        {
        }
    }
}