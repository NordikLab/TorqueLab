//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// TorqueLab Plugin Tools Container (Side settings area)
//==============================================================================

//==============================================================================
//Called from Toolbar and TerrainManager
function Lab::initGuiSystem(%this) {
	
}
//------------------------------------------------------------------------------

//==============================================================================
//Called from Toolbar and TerrainManager
function Lab::launchGuiSystem(%this) {
	//GlobalSceneTree.rebuild();
	
	FW.postEditorWake();
}
//------------------------------------------------------------------------------