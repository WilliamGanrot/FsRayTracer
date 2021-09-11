namespace RayTracer.Object

open System
open RayTracer.Transformation
open RayTracer.Matrix
open RayTracer.Vector
open RayTracer.Point
open RayTracer.Material
open RayTracer.Pattern
open RayTracer.Color
open RayTracer.Helpers
open RayTracer.Light
open RayTracer.Constnats


[<AutoOpen>]
module Domain =
    type Sphere = { radii: float; }

    type Shape =
        | Sphere of Sphere
        | Plane
        | Cube
        | Cylinder of minimum: float * maximum: float * closed: bool
        | Cone of minimum: float * maximum: float * closed: bool

    type Object =
        { transform: Matrix; transformInverse: Matrix; material: Material; shape: Shape; id: int}

        //compares object without id
         static member (.=.) (p, v : Object) =
            {p with id = 0} = {v with id = 0}
        
module Object =



    let sphere() =
        let sphere = Sphere { radii = 1. }
        { transform = Matrix.identityMatrix 4;
          transformInverse = Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = sphere; id = newRandom() }

    let glassSphere() =
        let sphere = Sphere { radii = 1. }

        let m =
            Material.standard
            |> Material.toGlass

        { transform = Matrix.identityMatrix 4;
          transformInverse = Matrix.identityMatrix 4 |> Matrix.inverse;
          material = m;
          shape = sphere;
          id = newRandom()}

    let plane() =
        { transform = Matrix.identityMatrix 4;
          transformInverse = Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Plane;
          id = newRandom();}

    let cube() =
        { transform = Matrix.identityMatrix 4;
          transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Cube;
          id = newRandom(); }

    let cylinder() =
        { transform = Matrix.identityMatrix 4;
          transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Cylinder(-infinity, infinity, false);
          id = newRandom(); }

    let cone() =
        { transform = Matrix.identityMatrix 4;
          transformInverse= Matrix.identityMatrix 4 |> Matrix.inverse;
          material = Material.standard;
          shape = Cone(-infinity, infinity, false);
          id = newRandom(); }

    let transform t object =
        let t = Transformation.applyToMatrix t object.transform
        { object with transform = t; transformInverse = t |> Matrix.inverse }

    let normal point object =

        let objectPoint =
            object.transformInverse
            |> Matrix.multiplyPoint point

        let objectNormal =
            match object.shape with
            | Sphere _ ->
                (objectPoint - (Point.create 0. 0. 0.))
            | Plane ->
                Vector.create 0. 1. 0.
            | Cube ->
                let maxc =
                    [objectPoint.X; objectPoint.Y; objectPoint.Z]
                    |> List.map (fun v -> Math.Abs(v:float))
                    |> List.max

                match maxc with
                | _ when (FloatHelper.equal maxc (Math.Abs(objectPoint.X:float))) -> Vector.create objectPoint.X 0. 0.
                | _ when (FloatHelper.equal maxc (Math.Abs(objectPoint.Y:float))) -> Vector.create 0. objectPoint.Y 0.
                | _                                                         -> Vector.create 0. 0. objectPoint.Z
            | Cylinder (min, max, _) ->
                let dist = objectPoint.X * objectPoint.X + objectPoint.Z * objectPoint.Z

                match dist < 1. with
                | true when objectPoint.Y >= max - epsilon -> Vector.create 0. 1. 0.
                | true when objectPoint.Y <= min + epsilon -> Vector.create 0. -1. 0.
                | _ -> Vector.create objectPoint.X 0. objectPoint.Z
            | Cone (min, max, _) ->
                let dist = objectPoint.X * objectPoint.X + objectPoint.Z * objectPoint.Z

                match dist < 1. with
                | true when objectPoint.Y >= max - epsilon -> Vector.create 0. 1. 0.
                | true when objectPoint.Y <= min + epsilon -> Vector.create 0. -1. 0.
                | _ ->
                    let y = Math.Sqrt(objectPoint.X * objectPoint.X + objectPoint.Z * objectPoint.Z)
                    let y' = if objectPoint.Y > 0. then -y else y
                    Vector.create objectPoint.X y' objectPoint.Z

        let worldNormal =
            object.transformInverse
            |> Matrix.Transpose
            |> Matrix.multiplyVector objectNormal
        
        {worldNormal with W = 0.} |> Vector.normalize

    let setMaterial m object =
        {object with material = m}

    let pattern (pattern:Pattern) object worldPoint =
        let objectPoint =
            object.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint worldPoint

        let patternPoint =
            pattern.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint objectPoint

        Pattern.at patternPoint pattern

    let lighting material light point eyevector normalv inShadow object =

        let color =
            match material.pattern with
            | Some p        -> pattern p object point 
            | None          -> material.color
        
        let effectiveColor = color * light.intensity
        let lightv = (light.poistion - point) |> Vector.normalize
        let ambient = effectiveColor |> Color.mulitplyByScalar material.ambient
        let lightDotNormal = Vector.dot lightv normalv

        let diffuse =
            effectiveColor
            |> Color.mulitplyByScalar (material.diffuse * lightDotNormal)

        let reflectDotEye =
            Vector.reflect normalv (lightv * -1.)
            |> Vector.dot eyevector

        match inShadow with
        | true -> ambient
        | false ->
            match lightDotNormal with
            | v when v < 0. -> ambient
            | _ when reflectDotEye <= 0. -> ambient + diffuse
            | _ ->
                let factor = Math.Pow(reflectDotEye, material.shininess)
                ambient + diffuse + (light.intensity |> Color.mulitplyByScalar (material.specular * factor))


