namespace RayTracer.Camera

open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Ray
open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.World
open Raytracer.Canvas


[<AutoOpen>]
module Domain =

    type Camera =
        { hsize: int;
          vsize: int;
          fov: float;
          transform: Matrix;
          pixelSize: float;
          halfWidth: float;
          halfHeight: float }

module Camera =

    let create hsize vsize fov =

        let (halfwidth, halfheight) =
            let halfView = Math.Tan(fov / 2.)
            let aspect = (float hsize) / (float vsize)

            match aspect >= 1. with
            | true -> (halfView, (halfView / aspect))
            | false -> ((halfView * aspect), halfView)
        
        { hsize = hsize;
          vsize = vsize;
          fov = fov;
          transform = Matrix.identityMatrix 4;
          halfWidth = halfwidth;
          halfHeight = halfheight;
          pixelSize = (halfwidth * 2.) / (float hsize)}

    let rayForPixel px py c =
        let xoffset = (px + 0.5) * c.pixelSize
        let yoffset = (py + 0.5) * c.pixelSize

        let wolrdX = (c.halfWidth - xoffset)
        let wolrdY = (c.halfHeight - yoffset)

        let pixel =
            c.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint (Point.create wolrdX wolrdY -1.)

        let origin =
            c.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint (Point.create 0. 0. 0.)

        let direction = Vector.normalize (pixel - origin)
        Ray.create origin direction

    let withTransfom t c =
        {c with transform = t}

    let pixels camera =
        [ for y in 0..camera.vsize - 1 do
            for x in 0..camera.hsize - 1 do
                (x,y) ]

    let render (world:World) (camera:Camera) =

        let image = Canvas.makeCanvas camera.hsize camera.vsize

        camera
        |> pixels
        |> List.map(fun (x, y) ->
            async {
                printfn "%A %A" y x
                let color = rayForPixel (float x) (float y) camera |> World.colorAt world World.maxDepth
                return (x,y,color)
            })
        |> Async.Parallel
        |> Async.RunSynchronously
        |> Array.toList
        |> List.iter (fun (x,y,c) -> Canvas.setPixel x y c image)

        image 
        



        

    
