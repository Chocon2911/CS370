using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountDbData : DbData
{
    public string Name { get; set; }

    public AccountDbData(string id, string name) : base(id)
    {
        this.Name = name;
    }

    public AccountDbData() : base() {}
}
