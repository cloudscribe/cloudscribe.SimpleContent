using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Web.Design
{
    public class SimpleContentIconConfig
    {
        public SimpleContentIconConfig()
        {
            IconSets = new List<IconCssClasses>();
        }

        public string DefaultSetId { get; set; } = "glyphicons";

        public List<IconCssClasses> IconSets { get; set; }

        public IconCssClasses GetIcons(string setId)
        {
            foreach (var set in IconSets)
            {
                if (set.SetId == setId)
                {
                    return set;
                }
            }

            foreach (var set in IconSets)
            {
                if (set.SetId == DefaultSetId)
                {
                    return set;
                }
            }

            return new IconCssClasses();
        }


    }
}
