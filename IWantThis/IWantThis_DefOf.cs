using RimWorld;

namespace IWantThis
{

    [DefOf]
    public class IWantThis_DefOf
    {
        public static QuestScriptDef IWantThis_BountyQuest;

        static IWantThis_DefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(IWantThis_DefOf));
        }
    }
}