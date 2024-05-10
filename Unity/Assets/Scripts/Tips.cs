namespace WumpusUnity
{
    public class Tips
    {
        public static Tips Instance => instance ??= new Tips();
        private static Tips instance;

        private bool hasBeenInVat = false;
        private bool hasBeenInAcrobat = false;
        private bool hasBeenInBat = false;

        public bool HasBeenInVat
        {
            get => hasBeenInVat;
            set => hasBeenInVat = value;
        }

        public bool HasBeenInAcrobat
        {
            get => hasBeenInAcrobat;
            set => hasBeenInAcrobat = value;
        }

        public bool HasBeenInBat
        {
            get => hasBeenInBat;
            set => hasBeenInBat = value;
        }
    }
}