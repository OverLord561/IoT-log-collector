﻿using Emulator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator.Config.Interfaces
{
    public interface IEmulatorConfiguration
    {
        ServerSettings GetServerSettings();
    }
}
