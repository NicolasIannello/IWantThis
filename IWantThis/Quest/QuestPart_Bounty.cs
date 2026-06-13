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
            if (signal.tag == inSignal+".pickupShipThing.SentSatisfied")
            {
                WorldComponent_IWantThis.Instance.ActiveBounty = false;
                WorldComponent_IWantThis.Instance.Reputation += WorldComponent_IWantThis.Instance.ReputationCompleted;
                Messages.Message("IWantThis.ReputationChange".Translate(WorldComponent_IWantThis.Instance.Reputation), MessageTypeDefOf.NeutralEvent);
            }
            if (signal.tag == inSignal + ".pickupShipThing.Destroyed")
            {
                WorldComponent_IWantThis.Instance.ActiveBounty = false;
                WorldComponent_IWantThis.Instance.Reputation += WorldComponent_IWantThis.Instance.ReputationDestroyed;
                Messages.Message("IWantThis.ReputationChange".Translate(WorldComponent_IWantThis.Instance.Reputation), MessageTypeDefOf.NegativeEvent);
                if (WorldComponent_IWantThis.Instance.Reputation < -4) WorldComponent_IWantThis.Instance.Reputation -= WorldComponent_IWantThis.Instance.ReputationDestroyed;
            }
            if (signal.tag == inSignal + ".pickupShipThing.SentUnsatisfied")
            {
                WorldComponent_IWantThis.Instance.ActiveBounty = false;
                WorldComponent_IWantThis.Instance.Reputation += WorldComponent_IWantThis.Instance.ReputationUnsatisfied;
                Messages.Message("IWantThis.ReputationChange".Translate(WorldComponent_IWantThis.Instance.Reputation),MessageTypeDefOf.RejectInput);
                if (WorldComponent_IWantThis.Instance.Reputation < -4) WorldComponent_IWantThis.Instance.Reputation -= WorldComponent_IWantThis.Instance.ReputationDestroyed;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref inSignal, nameof(inSignal));
        }
    }
}