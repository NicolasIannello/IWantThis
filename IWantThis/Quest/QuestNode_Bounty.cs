using RimWorld.QuestGen;
using Verse;

namespace IWantThis.Quest
{
    public class QuestNode_BountyStart : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> inSignal;

        protected override void RunInt()
        {
            WorldComponent_IWantThis.Instance.ActiveBounty = true;
            QuestPart_Bounty questPart = new QuestPart_Bounty();

            int questId = QuestGen.quest.id;
            questPart.inSignal = "Quest"+questId.ToString();

            QuestGen.quest.AddPart(questPart);
        }

        protected override bool TestRunInt(Slate slate) => true;
    }
}