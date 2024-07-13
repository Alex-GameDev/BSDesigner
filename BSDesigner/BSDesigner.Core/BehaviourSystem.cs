using System;
using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Core
{
    public class BehaviourSystem : IBehaviour
    {
        public List<BehaviourEngine> engines = new List<BehaviourEngine>();

        public BehaviourEngine Main
        {
            get
            {
                return engines.FirstOrDefault();
            }
            set
            {
                if(engines.Contains(value))
                {
                    engines.Remove(value);
                }
                engines.Insert(0, value);
            }
        }

        public void Pause()
        {
            this.Main.Pause();
        }

        public void SetContext(ExecutionContext context)
        {
           this.Main.SetContext(context);
        }

        public void Start()
        {
            this.Main.Start();
        }

        public void Stop()
        {
            this.Main.Stop();
        }

        public Status Update()
        {
            return this.Main.Update();
        }
    }
}
