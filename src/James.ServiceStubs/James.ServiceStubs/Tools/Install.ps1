param($installPath, $toolsPath, $package, $project)

$file1 = $project.ProjectItems.Item("routes.json")
$file2 = $project.ProjectItems.Item("Templates").ProjectItems.Item("Sample.template")

$copyAlways = [int]1
$copyIfNewer = [int]2
$copyToOutput1 = $file1.Properties.Item("CopyToOutputDirectory")
$copyToOutput1.Value = $copyAlways

$copyToOutput2 = $file2.Properties.Item("CopyToOutputDirectory")
$copyToOutput2.Value = $copyAlways