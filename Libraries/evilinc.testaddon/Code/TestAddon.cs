using Sandbox;
using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Editor;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sandbox.Network;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sandbox.Internal;
using System.Numerics;
using System.Globalization;
using Sandbox.Utility;
using System.Runtime.CompilerServices;

namespace TestAddon;

public class TestAddon : Component
{
	protected override void OnUpdate() 
	{
		Log.Info("Hello ADDONS");
	}
}
