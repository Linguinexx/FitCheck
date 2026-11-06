# Menu inicial anterior

SaveSlots.cs.txt contém o arquivo original, com o método TitleUI e o
comportamento anterior de ReturnToTitle.

Para restaurar, use os métodos TitleUI e ReturnToTitle da cópia. Remova
os campos titleScrim, titleAction e titleKeyboardFocus e os métodos
DrawTitleScrim, ReadTitleNavigation e TitleButton de SaveSlots.cs.
Remova também apenas a instrução que libera titleScrim no método
OnDestroy de CampaignInterface.cs.
Preserve as demais mudanças feitas no código desde esta cópia.
