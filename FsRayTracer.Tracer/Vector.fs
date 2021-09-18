namespace RayTracer.Vector
open RayTracer.Helpers
open RayTracer.RayDomain
open System



 
    module Vector =

        let create x y z : Vector =
            {X = x; Y = y; Z = z; W = 0.0}

        let multiblyByScalar (v:Vector) = v * 3.5

        let multiblyByFraction (v:Vector) = v * 0.5

        let divideByscalar (v:Vector) = v / 3.5

        let magnitude (v:Vector) : float = Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z)

        let normalize (v1:Vector) : Vector =
            let mag = magnitude v1

            { X = v1.X /. mag
              Y = v1.Y /. mag
              Z = v1.Z /. mag
              W = v1.W /. mag }

        let cross (a:Vector) (b:Vector) : Vector =
            create ((a.Y * b.Z) - (a.Z * b.Y)) ((a.Z * b.X) - (a.X * b.Z)) ((a.X * b.Y) - (a.Y * b.X))

        let dot (a:Vector) (b:Vector) =
            (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z) + (a.W * b.W)

        let reflect normal v = 
            v - (normal * 2. * dot v normal)

        let withW w vector : Vector = 
            { vector with W = w }

        