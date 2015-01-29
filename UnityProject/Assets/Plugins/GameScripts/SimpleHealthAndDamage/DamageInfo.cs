namespace SimpleLibrary
{
    public class DamageInfo
    {
        public UnityEngine.Transform Sender = null;
        public float Value = 0f;

        public DamageInfo(float value, UnityEngine.Transform sender)
        {
            Value = value;
            Sender = sender;
        }
    }
}
