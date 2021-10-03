module BoundingBoxTests
open RayTracer.OBJFile
open Xunit
open RayTracer.Triangle
open RayTracer.ObjectDomain
open RayTracer.RayDomain
open RayTracer.Point
open RayTracer.Group
open RayTracer.Helpers
open RayTracer.BoundingBox
open RayTracer.Sphere
open RayTracer.Plane
open RayTracer.Cube
open RayTracer.Cylinder
open RayTracer.Cone
open RayTracer.Triangle
open RayTracer.Object
open RayTracer.Vector
open RayTracer.Ray
open System


//[<Fact>]
//let ``Creating an empty bounding box``() =
//    let box = BoundingBox.create None
//    Point.equal box.min (Point.create infinity infinity infinity) |> Assert.True
//    Point.equal box.max (Point.create -infinity -infinity -infinity) |> Assert.True

[<Fact>]
let ``Creating a bounding box with volume``() =
    let p1 = (Point.create -1. -2. -3.)
    let p2 =(Point.create 3. 2. 1.)
    let box = BoundingBox.create (Some(p1, p2))

    Point.equal box.min p1 |> Assert.True
    Point.equal box.max p2 |> Assert.True

[<Fact>]
let ``Adding points to an empty bounding box``() =
    let box = BoundingBox.create None
    let p1 = (Point.create -5. 2. 0.)
    let p2 =(Point.create 7. 0. -3.)

    let box' = BoundingBox.addPoint p1 box
    let box'' = BoundingBox.addPoint p2 box'
    let boxe = box''

    Point.equal box''.min (Point.create -5. 0. -3.) |> Assert.True
    Point.equal box''.max (Point.create 7. 2. 0.) |> Assert.True

[<Fact>]
let ``A sphere has a bounding box``() =
    let s = Sphere.create()
    let box = BoundingBox.boundsOf s.shape

    Point.equal box.min (Point.create -1. -1. -1.) |> Assert.True
    Point.equal box.max (Point.create 1. 1. 1.) |> Assert.True

[<Fact>]
let ``A plane has a bounding box``() =
    let s = Plane.create()
    let box = BoundingBox.boundsOf s.shape

    Point.equal box.min (Point.create -infinity 0. -infinity) |> Assert.True
    Point.equal box.max (Point.create infinity 0. infinity) |> Assert.True


[<Fact>]
let ``A cube has a bounding box``() =
    let s = Cube.create()
    let box = BoundingBox.boundsOf s.shape

    Point.equal box.min (Point.create -1. -1. -1.) |> Assert.True
    Point.equal box.max (Point.create 1. 1. 1.)

[<Fact>]
let ``An unbounded cylinder has a bounding box``() =
    let s = Cylinder.create()
    let box = BoundingBox.boundsOf s.shape

    Point.equal box.min (Point.create -1. -infinity -1.) |> Assert.True
    Point.equal box.max (Point.create 1. infinity 1.) |> Assert.True

[<Fact>]
let ``A bounded cylinder has a bounding box``() =
    let s = Cylinder.build(Cylinder(-5., 3., false))
    let box = BoundingBox.boundsOf s.shape

    Point.equal box.min (Point.create -1. -5. -1.) |> Assert.True
    Point.equal box.max (Point.create 1. 3. 1.) |> Assert.True

[<Fact>]
let ``An unbounded cone has a bounding box``() =
    let s = Cone.create()
    let box = BoundingBox.boundsOf s.shape

    Point.equal box.min (Point.create -infinity -infinity -infinity) |> Assert.True
    Point.equal box.max (Point.create infinity infinity infinity) |> Assert.True

[<Fact>]
let ``A bounded cone has a bounding box``() =
    let s = Cone.build(Cone(-5., 3., false))
    let box = BoundingBox.boundsOf s.shape

    Point.equal box.min (Point.create -5. -5. -5.) |> Assert.True
    Point.equal box.max (Point.create 5. 3. 5.) |> Assert.True

[<Fact>]
let ``A triangle has a bounding box``() =
    let p1 = Point.create -3. 7. 2.
    let p2 = Point.create 6. 2. -4.
    let p3 = Point.create 2. -1. -1.

    let s = Triangle.create (p1, p2, p3)
    let box = BoundingBox.boundsOf s.shape

    Point.equal box.min (Point.create -3. -1. -4.) |> Assert.True
    Point.equal box.max (Point.create 6. 7. 2.) |> Assert.True

[<Fact>]
let ``Adding one bounding box to another``() =
    let box1 = BoundingBox.create (Some((Point.create -5. 2. 0.),(Point.create 7. 4. 4.)))
    let box2 = BoundingBox.create (Some((Point.create 8. -7. -2.),(Point.create 14. 2. 8.)))

    let box = BoundingBox.addBox box2 box1 

    Point.equal box.min (Point.create -5. -7. -2.) |> Assert.True
    Point.equal box.max (Point.create 14. 4. 8.) |> Assert.True

[<Theory>]
[<InlineData(5., -2., 0., true)>]
[<InlineData(11., 4., 7., true)>]
[<InlineData(8., 1., 3., true)>]
[<InlineData(3., 0., 3., false)>]
[<InlineData(8., -4., 3., false)>]
[<InlineData(8., 1., -1., false)>]
[<InlineData(13., 1., 3., false)>]
[<InlineData(8., 5., 3., false)>]
[<InlineData(8., 1., 8., false)>]
let ``Checking to see if a box contains a given point``(x,y,z,result) =
    let box = BoundingBox.create (Some((Point.create 5. -2. 0.),(Point.create 11. 4. 7.)))
    let p = Point.create x y z

    (BoundingBox.containsPoint p box) = result |> Assert.True


[<Theory>]
[<InlineData(5., -2., 0., 11, 4., 7., true)>]
[<InlineData(6., -1., 1., 10, 3., 6., true)>]
[<InlineData(4., -3., -1., 10, 3., 6., false)>]
[<InlineData(6., -1., 1., 12, 5., 8., false)>]

let ``Checking to see if a box contains a given box``(minx,miny,minz,maxx,maxy,maxz,result) =
    let box = BoundingBox.create (Some((Point.create 5. -2. 0.),(Point.create 11. 4. 7.)))
    let box2 = BoundingBox.create (Some((Point.create minx miny minz),(Point.create maxx maxy maxz)))

    (BoundingBox.containsBox box2 box) = result |> Assert.True


[<Fact>]
let ``Transforming a bounding box``() =
    let box = BoundingBox.create (Some((Point.create -1. -1. -1.),(Point.create 1. 1. 1.)))

    let box' =
        box
        |> BoundingBox.transform (Rotation(Y, (Math.PI/4.)))
        |> BoundingBox.transform (Rotation(X, (Math.PI/4.)))


    
    Point.equal box'.min (Point.create -1.4142 -1.7071 -1.7071) |> Assert.True
    Point.equal box'.max (Point.create 1.4142 1.7071 1.7071) |> Assert.True


[<Theory>]
[<InlineData(5., 0.5, 0., -1., 0., 0., true)>]
[<InlineData(-5., 0.5, 0., 1., 0., 0., true)>]
[<InlineData(0.5, 5., 0., 0., -1., 0., true)>]
[<InlineData(0.5, -5., 0., 0., 1., 0., true)>]
[<InlineData(0.5, 0., 5., 0., 0., -1., true)>]
[<InlineData(0.5, 0., -5., 0., 0., 1., true)>]
[<InlineData(0., 0.5, 0., 0., 0., 1., true)>]
[<InlineData(-2., 0., 0., 2., 4., 6., false)>]
[<InlineData(0., 0., -2., 4., 6., 2., false)>]
[<InlineData(2., 0., 2., 0., 0., -1., false)>]
[<InlineData(0., 2., 2., 0., -1., 0., false)>]
[<InlineData(2., 2., 0., -1., 0., 0., false)>]
let ``Intersecting a ray with a bounding box at the origin``(ox, oy, oz, dx,dy,dz, result) =
    let box = BoundingBox.create (Some((Point.create -1. -1. -1.),(Point.create 1. 1. 1.)))

    let direction = Vector.create dx dy dz |> Vector.normalize
    let origin = Point.create ox oy oz
    let ray = Ray.create origin direction

    (BoundingBox.intersects ray box) = result |> Assert.True


[<Theory>]
[<InlineData(15., 1., 2., -1., 0., 0., true)>]
[<InlineData(-5., -1., 4., 1., 0., 0., true)>]
[<InlineData(7., 6., 5., 0., -1., 0., true)>]
[<InlineData(9., -5., 6., 0., 1., 0., true)>]
[<InlineData(8., 2., 12., 0., 0., -1., true)>]
[<InlineData(8., 1., 3.5, 0., 0., 1., true)>]
[<InlineData(9., -1., -8., 2., 4., 6., false)>]
[<InlineData(8., 3., -4., 6., 2., 4., false)>]
[<InlineData(9., -1., -2., 4., 6., 2., false)>]
[<InlineData(4., 0., 9., 0., 0., -1., false)>]
[<InlineData(8., 6., -1., 0., -1., 0., false)>]
[<InlineData(12., 5., 4., -1., 0., 0., false)>]
let ``Intersecting a ray with a non-cubic bounding box``(ox, oy, oz, dx,dy,dz, result) =

    let box = BoundingBox.create (Some((Point.create 5. -2. 0.),(Point.create 11. 4. 7.)))

    let direction = Vector.create dx dy dz |> Vector.normalize
    let origin = Point.create ox oy oz
    let ray = Ray.create origin direction

    (BoundingBox.intersects ray box) = result |> Assert.True


[<Fact>]
let ``Splitting a perfect cube``() =
    let box = BoundingBox.create (Some((Point.create -1. -4. -5.),(Point.create 9. 6. 5.)))

    let left, right = BoundingBox.split box
    
    Point.equal left.min (Point.create -1. -4. -5.) |> Assert.True
    Point.equal left.max (Point.create 4. 6. 5.) |> Assert.True
    Point.equal right.min (Point.create 4. -4. -5.) |> Assert.True
    Point.equal right.max (Point.create 9. 6. 5.) |> Assert.True


[<Fact>]
let ``Splitting an x-wide box``() =
    let box = BoundingBox.create (Some((Point.create -1. -2. -3.),(Point.create 9. 5.5 3.)))

    let left, right = BoundingBox.split box
    
    Point.equal left.min (Point.create -1. -2. -3.) |> Assert.True
    Point.equal left.max (Point.create 4. 5.5 3.) |> Assert.True
    Point.equal right.min (Point.create 4. -2. -3.) |> Assert.True
    Point.equal right.max (Point.create 9. 5.5 3.) |> Assert.True
    
[<Fact>]
let ``Splitting an y-wide box``() =
    let box = BoundingBox.create (Some((Point.create -1. -2. -3.),(Point.create 5. 8. 3.)))

    let left, right = BoundingBox.split box
    
    Point.equal left.min (Point.create -1. -2. -3.) |> Assert.True
    Point.equal left.max (Point.create 5. 3. 3.) |> Assert.True
    Point.equal right.min (Point.create -1. 3. -3.) |> Assert.True
    Point.equal right.max (Point.create 5. 8. 3.) |> Assert.True

[<Fact>]
let ``Splitting an z-wide box``() =
    let box = BoundingBox.create (Some((Point.create -1. -2. -3.),(Point.create 5. 3. 7.)))

    let left, right = BoundingBox.split box
    
    Point.equal left.min (Point.create -1. -2. -3.) |> Assert.True
    Point.equal left.max (Point.create 5. 3. 2.) |> Assert.True
    Point.equal right.min (Point.create -1. -2. 2.) |> Assert.True
    Point.equal right.max (Point.create 5. 3. 7.) |> Assert.True

    

        
//[<Fact>]
//let ``partitioning a groups children``() =
//    let s1 = Sphere.create() 
//    let s2 = Sphere.create() 

//    let g =
//        Group.create()
//        |> Groupmake [s1;s2]

//    let (left,right) = BoundingBox.partitionChildren g
//    0

    