namespace RayTracer.Point

open RayTracer.Vector
open RayTracer.Helpers
open RayTracer.RayDomain



    module Point =

        let create x y z : Point =
            {X = x; Y = y; Z = z; W = 1.0}

        let equal (p1:Point) (p2:Point) =
            (FloatHelper.equal p1.X p2.X) && (FloatHelper.equal p1.Y p2.Y) && (FloatHelper.equal p1.Z p2.Z) && (FloatHelper.equal p1.W p2.W)