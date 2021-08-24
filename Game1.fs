namespace MyGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Game1() as this =
    inherit Game()

    let graphics = new GraphicsDeviceManager(this)
    let mutable effect: BasicEffect =  Unchecked.defaultof<_>
    let mutable verts: VertexPositionColor[] = Array.empty
    let mutable buffer: VertexBuffer = Unchecked.defaultof<_>

    do this.IsMouseVisible <- true

    override this.Initialize() =
        this.Window.AllowUserResizing <- true
        graphics.PreferredBackBufferWidth <- 1024
        graphics.PreferredBackBufferHeight <- 768
        graphics.ApplyChanges()

        let vert color (x,y,z) = VertexPositionColor(Vector3(x,y,z), color)

        verts <- [|
            vert Color.Red (0.0f, 1.0f, 0.0f)
            vert Color.Blue (-1.0f, -1.0f, 0.0f)
            vert Color.Green (1.0f, -1.0f, 0.0f)
        |]

        effect <- new BasicEffect(this.GraphicsDevice)
        effect.Projection <- Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            this.GraphicsDevice.Viewport.AspectRatio,
            0.001f, 1000f)
        effect.View <- Matrix.CreateLookAt(Vector3(0f,0f,-3f), Vector3.Forward, Vector3.Up)
        effect.World <- Matrix.Identity
        effect.VertexColorEnabled <- true


        buffer <- new VertexBuffer(this.GraphicsDevice,VertexPositionColor.VertexDeclaration , 3, BufferUsage.WriteOnly)
        buffer.SetData(verts)

        base.Initialize()

//    override this.Update gameTime =
//
//        base.Update gameTime

    override this.Draw gameTime =
        this.GraphicsDevice.Clear Color.White


        for pass in effect.CurrentTechnique.Passes do
            pass.Apply()
            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, 1)

        base.Draw(gameTime)
