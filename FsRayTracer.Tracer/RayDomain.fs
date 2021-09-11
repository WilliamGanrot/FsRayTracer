namespace RayTracer.RayDomain
open RayTracer.Helpers

[<AutoOpen>]
module RayDomain = 

    type Vector =
        { X : float
          Y : float
          Z : float
          W : float }

        static member (+) (v1, v2) =
            { X = v1.X + v2.X
              Y = v1.Y + v2.Y
              Z = v1.Z + v2.Z
              W = v1.W + v2.W }

        static member (-) (v1, v2) =
            { X = v1.X - v2.X
              Y = v1.Y - v2.Y
              Z = v1.Z - v2.Z
              W = v1.W - v2.W }

        static member (*) ((v1:Vector), (value:float)) =
            { X = v1.X * value
              Y = v1.Y * value
              Z = v1.Z * value
              W = v1.W * value }

        static member (/) ((v1:Vector), (value:float)) =
            { X = v1.X / value
              Y = v1.Y / value
              Z = v1.Z / value
              W = v1.W / value }

        member this.Negate =
            { X = -this.X
              Y = -this.Y
              Z = -this.Z
              W = -this.W }
    
        static member (.=) (p, v : Vector) = FloatHelper.equal p.X v.X && FloatHelper.equal p.Y v.Y && FloatHelper.equal p.Z v.Z && p.W = v.W

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


        static member (-) (p1: Point, p2: Point) : Vector =
                { X = p1.X - p2.X
                  Y = p1.Y - p2.Y
                  Z = p1.Z - p2.Z
                  W = p1.W - p2.W }

    type Ray = {origin:Point; direction:Vector;}
