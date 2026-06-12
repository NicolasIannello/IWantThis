using RimWorld;
using Verse;

namespace IWantThis.Quest
{
    public class QuestPart_Bounty : QuestPart
    {
        public string inSignal;

        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);
            int rep = WorldComponent_IWantThis.Instance.Reputation;
            if (signal.tag == inSignal+".pickupShipThing.SentSatisfied")
            {
                WorldComponent_IWantThis.Instance.ActiveBounty = false;
                WorldComponent_IWantThis.Instance.Reputation = rep + 1;
                Messages.Message("IWantThis.ReputationChange".Translate(), MessageTypeDefOf.NeutralEvent);
            }
            if (signal.tag == inSignal + ".pickupShipThing.Destroyed")
            {
                WorldComponent_IWantThis.Instance.ActiveBounty = false;
                WorldComponent_IWantThis.Instance.Reputation = rep - 3;
                Messages.Message("IWantThis.ReputationChange".Translate(), MessageTypeDefOf.NegativeEvent);
            }
            if (signal.tag == inSignal + ".pickupShipThing.SentUnsatisfied")
            {
                WorldComponent_IWantThis.Instance.ActiveBounty = false;
                WorldComponent_IWantThis.Instance.Reputation = rep - 2;
                Messages.Message("IWantThis.ReputationChange".Translate(),MessageTypeDefOf.RejectInput);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref inSignal, nameof(inSignal));
        }
    }
}