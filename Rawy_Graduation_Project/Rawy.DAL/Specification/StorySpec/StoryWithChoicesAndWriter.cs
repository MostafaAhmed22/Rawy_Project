using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.DAL.Specification.StorySpec
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
		public StoryWithChoicesAndWriter(int id) : base(S => S.Id == id) 
        {
			Includes.Add(S => S.Writer);
			//Includes.Add(S => S.Choises);
		}
    }
}
