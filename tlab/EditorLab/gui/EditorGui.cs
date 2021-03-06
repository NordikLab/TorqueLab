//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

function EditorGui::onAdd( %this ) {	
		
}

function EditorGui::onPreEditorSave( %this ) {
   EWorldEditor.preSaveParent = EWorldEditor.getParent();
   guiGroup.add(EWorldEditor);
   
   return;
	newSimGroup("EditorGuiPreSaveGroup");
	/*
	ETerrainEditor.preSaveParent = ETerrainEditor.getParent();
	EWorldEditor.preSaveParent = EWorldEditor.getParent();
	LabPaletteBar.preSaveParent = LabPaletteBar.getParent();
	LabPluginBar.preSaveParent = LabPluginBar.getParent();
	   EditorGuiPreSaveGroup.add(ETerrainEditor);
   EditorGuiPreSaveGroup.add(EWorldEditor);
   EditorGuiPreSaveGroup.add(LabPaletteBar);
   EditorGuiPreSaveGroup.add(LabPluginBar);
*/
   
	foreach(%ctrl in EditorGuiMain)
	{			
		%ctrls = strAddWord(%ctrls,%ctrl.getId());
	}
	foreach$(%ctrl in %ctrls)	
		%ctrl.defaultParent.add(%ctrl);	
		
	EWorldEditor.selectHandle = "tlab/art/icons/default/SelectHandle";
	EWorldEditor.defaultHandle = "tlab/art/icons/default/DefaultHandle";
	EWorldEditor.lockedHandle = "tlab/art/icons/default/LockedHandle";
}
function EditorGui::onPostEditorSave( %this ) {
   EWorldEditor.preSaveParent.add( EWorldEditor);
   

}
