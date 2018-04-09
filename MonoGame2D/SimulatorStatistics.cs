namespace Riku_fighter
{
    public class SimulatorStatistics
    {
        private int dead;
        private int alive; 
        
        public SimulatorStatistics(int dead, int alive)
        {
            this.dead = dead;
            this.alive = alive;
        }

        public int getDead()
        {
            return dead;
        }
        public int getAlive()
        {
            return alive;
        }
    }
}