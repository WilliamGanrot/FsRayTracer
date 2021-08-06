module ShapeTests

open System
open RayTracer.Point
open RayTracer.Vector
open RayTracer.Ray
open RayTracer.Matrix
open RayTracer.Intersection
open RayTracer.Helpers
open RayTracer.Transformation

open Xunit
open RayTracer.Shape

[<Fact>]
let ``a spheres deafult transformation`` () =
    let s = Shape.sphere
    (s.transform) .= (Matrix.identityMatrix 4) |> Assert.True

[<Fact>]
let ``changing a phere 's transformation`` () =
    let s = Shape.sphere
    let t = Translation(2., 3., 4.)

    let s2 = Shape.transform t s

    s2.transform .= (Transformation.matrix t) |> Assert.True

[<Fact>]
let ``intersecting a scaled sphere with a ray`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let s = Shape.sphere

    let s2 = s |> Shape.transform (Scaling(2., 2., 2.))
    let xs = Ray.intersect s2 r

    xs.Length = 2 |> Assert.True
    (FloatHelper.equal xs.[0].t 3.) |> Assert.True
    (FloatHelper.equal xs.[1].t 7.) |> Assert.True

[<Fact>]
let ``intersecting a translated sphere with a ray`` () =
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let s = Shape.sphere

    let s2 = s |> Shape.transform (Translation(5., 0., 0.))
    let xs = Ray.intersect s2 r

    xs.Length = 0 |> Assert.True
