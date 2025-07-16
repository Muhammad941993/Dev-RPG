namespace RPG.savingSystem
{
    public interface Isaveable
    {
        public object CaptureState();
        public void RestoreState(object state);
    }
}