using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAPI.Library.Interfaces
{
    public interface IGraphAPIService
    {
   
        Task<bool> CheckGroupByGroupIdAndUserAsync(string groupId, string userObjectId);
    
    }
}
