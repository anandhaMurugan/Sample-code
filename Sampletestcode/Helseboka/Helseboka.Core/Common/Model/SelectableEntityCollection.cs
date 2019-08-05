using System;
using System.Linq;
using System.Collections.Generic;

namespace Helseboka.Core.Common.Model
{
    public class SelectableEntityCollection<T> : List<SelectableEntity<T>>
    {
        public SelectableEntityCollection(List<T> dataList, bool defaultSelectionValue)
        {
            foreach (var item in dataList)
            {
                Add(new SelectableEntity<T> { Entity = item, IsSelected = defaultSelectionValue });
            }
        }

        public List<T> GetEntityList()
        {
            return this.Select(x => x.Entity).ToList();
        }
    }

	public class SelectableEntity<T>
	{
        public T Entity { get; set; }
        public Boolean IsSelected { get; set; }
	}

}
