using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class StateMasterTO
    {
		

			#region

			Int32 id;
			String name;
			Int32 parentId;
			String code;
			#endregion

			#region Get Set

			/// <summary>
			/// Sanjay [2017-02-10] To Hold the Id of the Dropdown
			/// </summary>
			public int Id
			{
				get
				{
					return id;
				}

				set
				{
					this.id = value;
				}
			}

			/// <summary>
			/// Sanjay [2017-02-10] To Hold the Text to be shown in dropdown
			/// </summary>
			public string Name
			{
				get
				{
					return name;
				}

				set
				{
					name = value;
				}
			}

			public Int32 ParentId
			{
				get
				{
					return parentId ;
				}

				set
				{
					parentId = value;
				}
			}

		public string Code { get => code; set => code = value; }



		#endregion
	}
	}


