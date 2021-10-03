namespace RayTracer.BoundingBox
open RayTracer.ObjectDomain
open RayTracer.RayDomain
open RayTracer.Point
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Cube

module BoundingBox =

    let create (t: Option<(Point*Point)>) : BoundingBox =
        match t with
        | Some (min,max) -> {min = min; max = max;}
        | None ->
            let min = Point.create infinity infinity infinity
            let max = Point.create -infinity -infinity -infinity
            {min = min; max = max;}

    let addPoint point boundingBox =
        let min = 
            let x = if point.X < boundingBox.min.X then point.X else boundingBox.min.X
            let y = if point.Y < boundingBox.min.Y then point.Y else boundingBox.min.Y
            let z = if point.Z < boundingBox.min.Z then point.Z else boundingBox.min.Z
            Point.create x y z

        let max = 
            let x = if point.X > boundingBox.max.X then point.X else boundingBox.max.X
            let y= if point.Y > boundingBox.max.Y then point.Y else boundingBox.max.Y
            let z = if point.Z > boundingBox.max.Z then point.Z else boundingBox.max.Z
            Point.create x y z

        create (Some(min,max))

    let addBox (childBox:BoundingBox) parentBox =
        parentBox
        |> addPoint childBox.min
        |> addPoint childBox.max


    let rec parentSpaceBoundsOf object =
        boundsOf object
        |> transform (Matrix(object.transform))

    and boundsOf (object:Object) : BoundingBox =
        match object.shape with
        | Sphere -> create (Some((Point.create -1. -1. -1.), (Point.create 1. 1. 1.)))
        | Plane -> create (Some((Point.create -infinity 0. -infinity), (Point.create infinity 0. infinity)))
        | Cube -> create (Some((Point.create -1. -1. -1.), (Point.create 1. 1. 1.)))
        | Cylinder(min,max,closed)->
            let min' = Point.create -1. min -1.
            let max' = Point.create 1. max 1.
            create (Some(min', max'))
        | Cone(min,max,closed) ->
            let a = abs min
            let b = abs max
            let limit = [a;b] |> List.max

            let min' = Point.create -limit min -limit
            let max' = Point.create limit max limit
            create (Some(min', max'))
        | Traingle(p1,p2,p3,e1,e2,n) ->
            create None
            |> addPoint p1
            |> addPoint p2
            |> addPoint p3
        | SmoothTraingle(p1,p2,p3,e1,e2,n1,n2,n3) ->
            create None
            |> addPoint p1
            |> addPoint p2
            |> addPoint p3
        | Group children ->

            //let mutable box = create None
            //for child in children do
            //    let cbox = parentSpaceBoundsOf child
            //    box <- addBox cbox box
                
            //box
            children
            |> List.map (fun c -> parentSpaceBoundsOf c)
            |> List.fold (fun box cbox -> addBox cbox box) (create None)


    and transform transformation box =
        let matrix = Transformation.matrix transformation

        let p1 = box.min
        let p2 = Point.create box.min.X box.min.Y box.max.Z
        let p3 = Point.create box.min.X box.max.Y box.min.Z
        let p4 = Point.create box.min.X box.max.Y box.max.Z
        let p5 = Point.create box.max.X box.min.Y box.min.Z
        let p6 = Point.create box.max.X box.min.Y box.max.Z
        let p7 = Point.create box.max.X box.max.Y box.min.Z
        let p8 = box.max


        [p1;p2;p3;p4;p5;p6;p7;p8]
        |> List.map (fun p -> Matrix.multiplyPoint p matrix)
        |> List.fold(fun p o -> addPoint o p) (create None)

    let intersects ray box =


        let (xtmin, xtmax) = Cube.checkAxis ray.origin.X ray.direction.X box.min.X box.max.X
        let (ytmin, ytmax) = Cube.checkAxis ray.origin.Y ray.direction.Y box.min.Y box.max.Y
        let (ztmin, ztmax) = Cube.checkAxis ray.origin.Z ray.direction.Z box.min.Z box.max.Z

        let tmin = [xtmin; ytmin; ztmin] |> List.max
        let tmax = [xtmax; ytmax; ztmax] |> List.min

        tmin <= tmax



    let containsPoint (point:Point) box =
        point.X >= box.min.X && point.X <= box.max.X &&
        point.Y >= box.min.Y && point.Y <= box.max.Y &&
        point.Z >= box.min.Z && point.Z <= box.max.Z

    let containsBox childBox box = (containsPoint childBox.min box) && (containsPoint childBox.max box)
