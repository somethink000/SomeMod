// Created by Crowbar 0.74

$modelname "weapons\fas2\world\rifles\ak400.mdl"

$bodygroup "weapon"
{
	studio "ak400.smd"
}
$bodygroup "Barrel"
{
	studio "muzzle.smd"
	studio "suppressor.smd"
	studio "muzzlebrake.smd"
	studio "flashhider.smd"
}
$bodygroup "Magazine"
{
	studio "30rnd.smd"
	studio "45rnd.smd"
	studio "60rnd.smd"
}
$bodygroup "Grip"
{
	blank
	studio "grip.smd"
}


$surfaceprop "weapon"

$contents "solid"

$illumposition -0.261 -0.093 3.649

$cdmaterials "models\weapons\fas2\rifles\ak400\"
$cdmaterials "models\weapons\fas2\rifles\m4a1\"
$cdmaterials "models\weapons\fas2\accessories\"
$cdmaterials "models\weapons\shells\"

$attachment "muzzle" "muzzle" 0 0 0 rotate 0 0 0
$attachment "eject" "eject" 0 0 0 rotate -15 -90 90

$cbox 0 0 0 0 0 0

$bbox -22.257 -2.027 -7.498 22.443 1.353 14.796

$definebone "ValveBiped.Bip01_R_Hand" "" -0.678304 13.183071 4.586786 10.000159 -90.000016 -180.000005 0 0 0 0 0 0
$definebone "muzzle" "ValveBiped.Bip01_R_Hand" 29.93125 -0.79168 -7.69328 10.000178 0 -179.999978 0 0 0 0 0 0
$definebone "eject" "ValveBiped.Bip01_R_Hand" 9.573913 -0.33549 -4.831962 10.000178 0 -179.999978 0 0 0 0 0 0

$bonemerge "ValveBiped.Bip01_R_Hand"
$bonemerge "muzzle"
$bonemerge "eject"


$sequence "idle" {
	"ak400_anims\idle.smd"
	fadein 0.2
	fadeout 0.2
	fps 30
}

$collisionmodel "ak400_physics.smd"
{
	$mass 8
	$inertia 1
	$damping 0
	$rotdamping 0
	$rootbone " "

}
