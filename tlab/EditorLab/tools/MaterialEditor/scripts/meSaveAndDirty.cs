//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Material Update Functionality
$Pref::LabMat::DefaultMaterialFile = "art/materials.cs";
function LabMat::saveDialogCancel( %this ) {
	LabMat.guiSync( materialEd_previewMaterial );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::saveDialogDontSave( %this, %material ) {
	LabMat.currentMaterial.setName( %this.originalName );
	//restore to defaults
	LabMat.copyMaterials( notDirtyMaterial, LabMat.currentMaterial );
	LabMat.copyMaterials( notDirtyMaterial, materialEd_previewMaterial );
	LabMat.guiSync( materialEd_previewMaterial );
	materialEd_previewMaterial.flush();
	materialEd_previewMaterial.reload();
	LabMat.currentMaterial.flush();
	LabMat.currentMaterial.reload();
	LabMat.setMaterialNotDirty();
	LabMat.setActiveMaterial( %material );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::saveDialogSave( %this, %material ) {
	LabMat.save();
	LabMat.setActiveMaterial( %material );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::save( %this,%material,%external ) {
	%mat = LabMat.currentMaterial;

	if (!isObject(%mat))
		return;

	if( %mat.getName() $= "" ) {
		LabMsgOK("Cannot perform operation", "Saved materials cannot be named \"\". A name must be given before operation is performed" );
		return;
	}

	// Update the live object regardless in this case
	LabMat.updateLivePreview(true);

	// Specifically for materials autogenerated from shapes.
	if( %mat.isAutoGenerated() )
		%mat.setAutoGenerated( false );
	%isDirty = matEd_PersistMan.isDirty(LabMat.currentMaterial);
	// Save the material using the persistence manager
	matEd_PersistMan.saveDirty();
	// Clean up the Material Editor
	LabMat.copyMaterials( materialEd_previewMaterial, notDirtyMaterial );
	LabMat.setMaterialNotDirty();
}
//------------------------------------------------------------------------------
//LabMat.setMaterialDirty
//==============================================================================
function LabMat::setMaterialNotDirty(%this) {
	%propertyText = "Material Properties";
	%previewText = "Material Preview";
	MaterialEditorPropertiesWindow.text = %propertyText;
	MaterialEditorPreviewWindow.text = %previewText;
	MaterialEditorPropertiesWindow-->quickSaveButton.active = 0;
	LabMat.materialDirty = false;
	matEd_PersistMan.removeDirty(LabMat.currentMaterial);
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::setMaterialDirty(%this) {
	%propertyText = "Material Properties *";
	%previewText = "Material Preview *";
	MaterialEditorPropertiesWindow.text = %propertyText;
	MaterialEditorPreviewWindow.text = %previewText;
	LabMat.materialDirty = true;
	MaterialEditorPropertiesWindow-->quickSaveButton.active = 1;

	// materials created in the material selector are given that as its filename, so we run another check
	if( LabMat.isLabMatitorMaterial( LabMat.currentMaterial ) ) {
		if( LabMat.currentMaterial.isAutoGenerated() ) {
			%obj = LabMat.currentObject;

			if( %obj.shapeName !$= "" )
				%shapePath = %obj.shapeName;
			else if( %obj.isMethod("getDatablock") ) {
				if( %obj.getDatablock().shapeFile !$= "" )
					%shapePath = %obj.getDatablock().shapeFile;
			}

			//creating toPath
			%k = 0;

			while( strpos( %shapePath, "/", %k ) != -1 ) {
				%pos = strpos( %shapePath, "/", %k );
				%k = %pos + 1;
			}

			%savePath = getSubStr( %shapePath , 0 , %k );
			%savePath = %savePath @ "materials.cs";
			matEd_PersistMan.setDirty(LabMat.currentMaterial, %savePath);
		} else {
			matEd_PersistMan.setDirty(LabMat.currentMaterial, "art/materials.cs");
		}
	} else
		matEd_PersistMan.setDirty(LabMat.currentMaterial);
}
//------------------------------------------------------------------------------

//==============================================================================
function LabMat::showSaveDialog( %this, %toMaterial ) {
	LabMsgYesNoCancel("Save Material?",
							"The material " @ LabMat.currentMaterial.getName() @ " has unsaved changes. <br>Do you want to save?",
							"LabMat.saveDialogSave(" @ %toMaterial @ ");",
							"LabMat.saveDialogDontSave(" @ %toMaterial @ ");",
							"LabMat.saveDialogCancel();" );
}
//------------------------------------------------------------------------------
//==============================================================================
function LabMat::showMaterialChangeSaveDialog( %this, %toMaterial ) {
	%fromMaterial = LabMat.currentMaterial;
	LabMsgYesNoCancel("Save Material?",
							"The material " @ %fromMaterial.getName() @ " has unsaved changes. <br>Do you want to save before changing the material?",
							"LabMat.saveDialogSave(" @ %toMaterial @ "); LabMat.changeMaterial(" @ %fromMaterial @ ", " @ %toMaterial @ ");",
							"LabMat.saveDialogDontSave(" @ %toMaterial @ "); LabMat.changeMaterial(" @ %fromMaterial @ ", " @ %toMaterial @ ");",
							"LabMat.saveDialogCancel();" );
}
//------------------------------------------------------------------------------
