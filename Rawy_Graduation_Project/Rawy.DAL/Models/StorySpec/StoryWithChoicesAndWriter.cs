using Rawy.DAL.Models;
using Rawy.DAL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Models.StorySpec
{
    public class StoryWithChoicesAndWriter : BaseSpecifications<Story>
    {

        // Constructor To Return All Stories With its NavigationProperty
        public StoryWithChoicesAndWriter() : base()
        {
            Includes.Add(S => S.Writer);
            // Includes.Add(S => S.Choises);
        }


        // This Constructor To Return Specific Story With its NavigationProperty
        public StoryWithChoicesAndWriter(string id) : base(S => S.Id == id)
        {
            Includes.Add(S => S.Writer);
            //Includes.Add(S => S.Choises);
        }
    }
}
