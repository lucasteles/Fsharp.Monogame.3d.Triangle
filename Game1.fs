namespace MyGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Game1() as this =
    inherit Game()

    let graphics = new GraphicsDeviceManager(this)
    let mutable effect = Unchecked.defaultof<BasicEffect>
    let mutable verts = Array.empty<VertexPositionColor>

    let vertex color struct (x, y, z) =
        VertexPositionColor(Vector3(x, y, z), color)

    override this.Initialize() =
        this.IsMouseVisible <- true
        this.Window.AllowUserResizing <- true
        graphics.PreferredBackBufferWidth <- 1024
        graphics.PreferredBackBufferHeight <- 768
        graphics.ApplyChanges()

        verts <-
            [| vertex Color.Red (0.0f, 1.0f, 0.0f)
               vertex Color.Blue (-1.0f, -1.0f, 0.0f)
               vertex Color.Green (1.0f, -1.0f, 0.0f) |]

        let fov =
            Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                this.GraphicsDevice.Viewport.AspectRatio,
                0.001f,
                1000f
            )

        effect <-
            new BasicEffect(
                device = this.GraphicsDevice,
                VertexColorEnabled = true,
                Projection = fov,
                View = Matrix.CreateLookAt(Vector3(0f, 0f, -3f), Vector3.Forward, Vector3.Up),
                World = Matrix.Identity
            )


        let buffer =
            new VertexBuffer(
                this.GraphicsDevice,
                VertexPositionColor.VertexDeclaration,
                verts.Length,
                BufferUsage.WriteOnly
            )

        buffer.SetData(verts)

        base.Initialize()

    override this.Draw gameTime =
        this.GraphicsDevice.Clear Color.White

        for pass in effect.CurrentTechnique.Passes do
            pass.Apply()
            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, 1)

        base.Draw(gameTime)
