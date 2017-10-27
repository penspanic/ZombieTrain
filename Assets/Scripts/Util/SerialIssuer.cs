using UnityEngine;
using System.Collections;

public class SerialIssuer
{
    private int _lastIssuedValue = 0;

    public int Get()
    {
        return ++_lastIssuedValue;
    }
}