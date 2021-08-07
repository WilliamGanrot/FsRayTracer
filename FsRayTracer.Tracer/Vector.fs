namespace RayTracer.Vector
open RayTracer.Helpers
open RayTracer.Helpers

    [<AutoOpen>]
    module Domain =

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

 
    module Vector =

        let create x y z =
            {X = x; Y = y; Z = z; W = 0.0}

        let multiblyByScalar (v:Vector) = v * 3.5

        let multiblyByFraction (v:Vector) = v * 0.5

        let divideByscalar (v:Vector) = v / 3.5

        let magnitude (v:Vector) : float = sqrt (v.X * v.X + v.Y * v.Y + v.Z * v.Z)

        let normalize (v1:Vector) : Vector =
            { X = v1.X / magnitude v1
              Y = v1.Y / magnitude v1
              Z = v1.Z / magnitude v1
              W = v1.W / magnitude v1 }

        let cross (a:Vector) (b:Vector) : Vector =
            create ((a.Y * b.Z) - (a.Z * b.Y)) ((a.Z * b.X) - (a.X * b.Z)) ((a.X * b.Y) - (a.Y * b.X))

        let dot a b =
            (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z) + (a.W * b.W)

        let reflect normal v = 
            v - (normal * 2. * dot v normal)

        