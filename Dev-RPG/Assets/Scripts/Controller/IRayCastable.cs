namespace RPG.Control
{
    public interface IRayCastable
    {
        public CursorType GetCursorType();
        public bool HandleRayCast(PlayerController player);

    }
}