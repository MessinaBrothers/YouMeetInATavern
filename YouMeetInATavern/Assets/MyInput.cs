﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyInput : DataUser {

    public abstract void Handle(string input);

    public abstract void Handle(float h, float v);
}
