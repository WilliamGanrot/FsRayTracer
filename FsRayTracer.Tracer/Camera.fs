namespace RayTracer.Camera

open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Ray
open System
open RayTracer.Point
open RayTracer.Vector


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