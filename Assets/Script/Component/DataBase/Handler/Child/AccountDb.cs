using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AccountDb : DatabaseHandler<AccountDbData>
{
    public virtual void RemoveRowByObjId(string id)
    {
        using (var connection = GetConnection())
        {
            var obj = connection.Table<AccountDbData>().FirstOrDefault(x => x.Id == id);
            if (obj != null) connection.Delete(obj);
        }
    }
}
