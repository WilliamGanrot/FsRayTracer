module WorldTests

open System
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Helpers
open RayTracer.Light
open RayTracer.Material
open RayTracer.Color
open RayTracer.World
open RayTracer.Shape
open RayTracer.Transformation
open RayTracer.Ray

open Xunit
open RayTracer.Intersection
open RayTracer.Computation

//[<Fact>]
//let ``creating a world``  () =
//    let w = World.empty
//    w.objects.Length = 0 |> Assert.True
//    w.light = None |> Assert.True


[<Fact>]
let ``the default world``  () =

    let w = World.standard

    let light = Light.create (Color.create 1. 1. 1.) (Point.create -10. 10. -10.)

    let material =
        Material.standard
        |> Material.withColor (Color.create 0.8 1.0 0.6)
        |> Material.withDiffuse 0.7
        |> Material.withSpecular 0.2

    let s1 =
        Shape.sphere
        |> Shape.setMaterial material

    let s2 =
        Shape.sphere
        |> Shape.transform (Scaling(0.5, 0.5, 0.5))

        
    w.objects.Length = 2 |> Assert.True
    w.objects |> List.exists (fun x -> x = s1) |> Assert.True
    w.objects |> List.exists (fun x -> x = s2) |> Assert.True
 
    w.light = light |> Assert.True

[<Fact>]
let ``intersect a world with a ray``  () =

    let w = World.standard
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)

    let xs = World.intersect w r

    xs.Length = 4 |> Assert.True
    xs.[0].t |> FloatHelper.equal 4. |> Assert.True
    xs.[1].t |> FloatHelper.equal 4.5 |> Assert.True
    xs.[2].t |> FloatHelper.equal 5.5 |> Assert.True
    xs.[3].t |> FloatHelper.equal 6. |> Assert.True




[<Fact>]
let ``shading an intersection`` () =
    let w = World.standard
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let shape = w.objects.Head
    let i = Intersection.create shape 4.0
    let comps = Computation.prepare r i

    let c = World.shadeHit w comps
    c .= Color.create 0.38066 0.47583 0.2855 |> Assert.True

[<Fact>]
let ``shading an intersection from the inside`` () =

    
    let w =
        World.standard
        |> World.withLight (Light.create (Color.create 1. 1. 1.) (Point.create 0. 0.25 0.))

    let r = Ray.create (Point.create 0. 0. 0.) (Vector.create 0. 0. 1.)
    let shape = w.objects.[1]
    let i = Intersection.create shape 0.5
    let comps = Computation.prepare r i

    let c = World.shadeHit w comps
    c .= Color.create 0.90498 0.90498 0.90498 |> Assert.True


[<Fact>]
let ``the color when a ray misses`` () =
    let w = World.standard
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 1. 0.)
    let c = World.colorAt w r

    c .= Color.black |> Assert.True

[<Fact>]
let ``the color when a ray hits`` () =
    let w = World.standard
    let r = Ray.create (Point.create 0. 0. -5.) (Vector.create 0. 0. 1.)
    let c = World.colorAt w r

    c .= Color.create 0.38066 0.47583 0.2855 |> Assert.True

[<Fact>]
let ``the color with an intersection behind the ray`` () =
    let wpre = World.standard
        
    let outer =
        wpre.objects.[0]
        |> Shape.setMaterial (wpre.objects.[0].material |> Material.withAmbient 1.)

    let inner =
        wpre.objects.[1]
        |> Shape.setMaterial (wpre.objects.[1].material |> Material.withAmbient 1.)

    let r = Ray.create (Point.create 0. 0. 0.75) (Vector.create 0. 0. -1.)

    let w = {wpre with objects = [outer;inner]}
    let c = World.colorAt w r

    c .= inner.material.color |> Assert.True

[<Fact>]
let ``there is not shadow when nothing is collinear with point and light`` () =
    let w = World.standard
    let p = Point.create 0. 10. 0.

    World.isShadowed p w |> Assert.False

[<Fact>]
let ``the shadow when an object is between the point and the light`` () =
    let w = World.standard
    let p = Point.create 10. -10. 10.

    World.isShadowed p w |> Assert.True

[<Fact>]
let ``there is no shadow when an object is behind the light`` () =
    let w = World.standard
    let p = Point.create -20. 20. -20.

    World.isShadowed p w |> Assert.False

[<Fact>]
let ``there is no shadow when an object is behind the point`` () =
    let w = World.standard
    let p = Point.create -2. 2. -2.

    World.isShadowed p w |> Assert.False

[<Fact>]
let ``shadeHit is given an intersection in shadow `` () =

    let s1 = Shape.sphere
    let s2 =
        Shape.sphere
        |> Shape.transform (Translation(0., 0., 10.))

    let w =
        World.empty
        |> World.withLight (Light.create (Color.create 1. 1. 1.) (Point.create 0. 0. -10.))
        |> World.addObject s1
        |> World.addObject s2

    let r = Ray.create (Point.create 0. 0. 5.) (Vector.create 0. 0. 1.)
    let i = Intersection.create s2 4.

    let comps = Computation.prepare r i
    let c = World.shadeHit w comps
    c .= Color.create 0.1 0.1 0.1 |> Assert.True




    


