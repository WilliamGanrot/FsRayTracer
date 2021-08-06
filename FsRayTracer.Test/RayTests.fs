module RayTests

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
let ``creating and quering a ray`` () =
    let o = Point.create 1. 2. 3.
    let d = Vector.create 4. 5. 6.

    let r = Ray.create o d

    r.direction .= d |> Assert.True

    Point.equal r.origin o
    |> Assert.True
    
[<Fact>]
let ``computing a point from a distance`` () =

    let r = Ray.create (Point.create 1. 2. 3.) (Vector.create 4. 5. 6.)

    Point.equal (Ray.position 0. r) (Point.create 2. 3. 4.)    
    Point.equal (Ray.position 1. r) (Point.create 3. 3. 4.)    
    Point.equal (Ray.position -1. r) (Point.create 1. 3. 4.)    
    Point.equal (Ray.position 2.5 r) (Point.create 4.5 3. 4.)
    
[<Fact>]
let ``a ray intersects a sphere at two points`` () =

    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)

    let s = Shape.sphere
    let xs = Ray.intersect s r

    xs.Length = 2 |> Assert.True
    FloatHelper.equal xs.[0].t 4. |> Assert.True
    FloatHelper.equal xs.[1].t 6. |> Assert.True

    
[<Fact>]
let ``a ray intersects a sphere at tangent`` () =

    let r = Ray.create (Point.create 0. 1. -5.) (Vector.create 0. 0. 1.)

    let s = Shape.sphere
    let xs = Ray.intersect s r

    xs.Length = 2 |> Assert.True
    FloatHelper.equal xs.[0].t 5. |> Assert.True
    FloatHelper.equal xs.[1].t 5. |> Assert.True

[<Fact>]
let ``a ray misses a sphere`` () =

    let r = Ray.create (Point.create 0. 2. -5.) (Vector.create 0. 0. 1.)

    let s = Shape.sphere
    let xs = Ray.intersect s r

    xs.Length = 0 |> Assert.True

[<Fact>]
let ``a ray originates inside a sphere`` () =

    let r = Ray.create (Point.create 0. 0. 0.) (Vector.create 0. 0. 1.)

    let s = Shape.sphere
    let xs = Ray.intersect s r

    xs.Length = 2 |> Assert.True
    FloatHelper.equal xs.[0].t -1. |> Assert.True
    FloatHelper.equal xs.[1].t 1. |> Assert.True


[<Fact>]
let ``a sphere is behind a ray`` () =

    let r = Ray.create (Point.create 0. 0. 5.) (Vector.create 0. 0. 1.)

    let s = Shape.sphere
    let xs = Ray.intersect s r

    xs.Length = 2 |> Assert.True
    FloatHelper.equal xs.[0].t -6. |> Assert.True
    FloatHelper.equal xs.[1].t -4. |> Assert.True

[<Fact>]
let ``translating a ray`` () =
    let r = Ray.create (Point.create 1. 2. 3.) (Vector.create 0. 1. 0.)
    let t = Translation(3., 4., 5.)

    let r2 = Ray.transform r t

    Point.equal (r2.origin) (Point.create 4. 6. 8.) |> Assert.True
    (r2.direction) .= (Vector.create 0. 1. 0.) |> Assert.True

[<Fact>]
let ``scaling a ray`` () =
    let r = Ray.create (Point.create 1. 2. 3.) (Vector.create 0. 1. 0.)
    let t = Scaling(2., 3., 4.)

    let r2 = Ray.transform r t

    Point.equal (r2.origin) (Point.create 2. 6. 12.) |> Assert.True
    (r2.direction) .= (Vector.create 0. 3. 0.) |> Assert.True
