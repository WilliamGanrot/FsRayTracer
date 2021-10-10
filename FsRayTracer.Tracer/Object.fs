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
open RayTracer.ObjectDomain
open RayTracer.RenderingDomain
open RayTracer.RayDomain
open RayTracer.BoundingBox
open RayTracer.Group



module Object =

    let transform t (object:Object) : Object =
        let t = Transformation.applyToMatrix t object.transform
        let object' = { object with transform = t; transformInverse = t |> Matrix.inverse }
        {object' with bounds = BoundingBox.parentSpaceBoundsOf object' }
        
        
    let setMaterial m object =
        {object with material = m}

    let pattern (pattern:Pattern) (object:Object) worldPoint =
        let objectPoint =
            object.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint worldPoint

        let patternPoint =
            pattern.transform
            |> Matrix.inverse
            |> Matrix.multiplyPoint objectPoint

        Pattern.at patternPoint pattern

    let lighting material light point eyevector normalv inShadow (object:Object) =

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

    let parent o (tree: Object list) =

        let rec loopNode (node: Object) =
            match node.shape with
            | _ when node.id = o.id -> Some node
            | Group(children) when children.Length <> 0 -> loopChildren children
            | Csg(_,l,r) -> loopChildren [l;r]
            | _ -> None

        and loopChildren (tree: Object list) =
            match tree with
            | h::t ->
                match loopNode h with
                | Some v -> Some h
                | None -> loopChildren t 

            | _ -> None

        loopChildren tree

    let gettrail (key:int) (tree:Object list) =
        let mutable mlist = []

        let rec loopNode (node: Object) =
            match node.shape with
            | _ when node.id = key -> Some node
            | Group(children) when children.Length <> 0 -> loopChildren children
            | Csg(_,left,right) -> loopChildren [left;right]
            | _ -> None
            
    
        and loopChildren (tree: Object list) =
            match tree with
            | h::t ->
                match loopNode h with
                | Some v ->
                    mlist <- mlist @ [h]
                    Some v
                | None -> loopChildren t 

            | _ -> None

        loopChildren tree |> ignore
        mlist

    let rec childIsIn child parent =
        match parent.shape with
        | Group(children) -> children |> List.exists(fun c -> childIsIn child c)
        | Csg(_,left,right) -> childIsIn child left || childIsIn child right
        | _ -> parent.id = child.id


    let worldToObject (object:Object) point topparents =

        gettrail object.id topparents
        |> List.rev
        |> List.fold (fun p o -> Matrix.multiplyPoint p o.transformInverse) point

    let normalToWorld (object:Object) (v:Vector) (topparents) : Vector =

        let handle (o:Object) n =
            o.transformInverse
            |> Matrix.Transpose
            |> Matrix.multiplyVector n
            |> Vector.withW 0.
            |> Vector.normalize

        gettrail object.id topparents
        |> List.fold (fun v o -> handle o v) v

    let normal point topparents (object:Object) (i: Intersection Option)=

        let localPoint = worldToObject object point topparents
        let localNormal = object.localNormalAt object.shape localPoint i

        normalToWorld object localNormal topparents

    let rec divide threshold object : Object =

        match object.shape with
        | Group(children) when threshold < children.Length ->
            let partitions = BoundingBox.partitionChildren object

            let partitionGroups =
                [partitions.left; partitions.right; partitions.rest]
                |> List.filter (fun x -> x.Length > 0)
                |> List.map (fun x -> Group.create() |> Group.setChildren x)

            match partitionGroups with
            | h::[] ->
                let children = Group.getChildren h
                Group.create() |> Group.setChildren children
            | _ ->
                partitionGroups
                |> List.fold(fun accGroup patrition ->
                    let divided = divide threshold patrition
                    Group.addChildren [divided] accGroup ) (Group.create())

        | _ -> object
