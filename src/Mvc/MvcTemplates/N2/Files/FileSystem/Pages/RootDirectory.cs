﻿using N2.Integrity;
using N2.Definitions;
using N2.Installation;
using N2.Web;

namespace N2.Edit.FileSystem.Items
{
    [PageDefinition("File Folder", 
		Description = "A node that maps to files in the file system.",
		SortOrder = 600, 
		InstallerVisibility = InstallerHint.NeverRootOrStartPage,
        IconUrl = "{ManagementUrl}/Resources/icons/folder.png",
		TemplateUrl = "{ManagementUrl}/Files/FileSystem/Directory.aspx")]
    [RestrictParents(typeof(IFileSystemContainer))]
    [ItemAuthorizedRoles("Administrators", "admin")]
    [Editables.EditableFolderPath]
	[Template("info", "{ManagementUrl}/Files/FileSystem/Directory.aspx")]
	[Disable]
    public class RootDirectory : AbstractDirectory
    {
        public RootDirectory()
        {
            Visible = false;
            SortOrder = 10000;
        }
    }
}
