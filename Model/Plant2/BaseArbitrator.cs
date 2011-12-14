using System;
using System.Collections.Generic;
using System.Text;
using CSGeneral;

[Description("A simple arbitrator of plant growth based upon simple source-sink relationships.")] 
abstract public class BaseArbitrator
   {
   abstract public double DMSupply {get;}   // property to provide total daily DM supply to organs
   abstract public double NDemand { get; } //property to provide total daily N demand to organs
   virtual public void DoDM(List<Organ> Organs){}  // method called by parent plant to tell arbitrator to do DM arbitration.
   virtual public void DoN(List<Organ> Organs) { }  // method called by parent plant to tell arbitrator to do N arbitration.
   }

