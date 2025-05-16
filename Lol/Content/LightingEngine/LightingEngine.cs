global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Terraria;
global using Terraria.ModLoader;
using Terraria.Graphics.Light;
using System.Runtime.CompilerServices;
using Vec3 = System.Numerics.Vector3;
using Vec4 = System.Numerics.Vector4;
using Vec2 = System.Numerics.Vector2;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Map;
using System.Reflection;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics.PackedVector;


namespace Lol.Content.LightingEngine
{
    public class LightingEngine : ModSystem
    {
        public override void Load()
        {
            // This is where you would initialize your lighting engine.
            // For example, you might set up shaders, load textures, etc.
        }
        public override void Unload()
        {
            // Clean up any resources used by your lighting engine.
        }
        public override void PostUpdateEverything()
        {
            // This is where you would update your lighting engine each frame.
            // For example, you might update the position of lights, etc.
        }
    }
}
