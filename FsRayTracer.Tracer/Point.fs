namespace RayTracer.Point

open RayTracer.Vector

    [<AutoOpen>]
    module Domain =
        type Point =
            {X: float; Y:float; Z:float; W:float}

            static member (+) (p: Point, v: Vector) =
                    { X = p.X + v.X
                      Y = p.Y + v.Y
                      Z = p.Z + v.Z
                      W = p.W + v.W }


                static member (-) (p: Point, v : Vector) =
                    { X = p.X - v.X
                      Y = p.Y - v.Y
                      Z = p.Z - v.Z
                      W = p.W - v.W }


                static member (-) (p1: Point, p2) : Vector =
                    { X = p1.X - p2.X
                      Y = p1.Y - p2.Y
                      Z = p1.Z - p2.Z
                      W = p1.W - p2.W }


    [<AutoOpen>]
    module Operations =

        let createPoint (x, y, z) =
            {X = x; Y = y; Z = z; W = 1.0}