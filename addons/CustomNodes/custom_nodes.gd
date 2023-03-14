@tool
extends EditorPlugin

const PATH = "res://CustomNodes"

var default_icon = preload("res://Assets/Images/CustomNodes/CustomControl.svg")

var last_selected_node: Node

var loaded_types: PackedStringArray


func _enter_tree():
	# Initialization of the plugin goes here.
	load_custom_types()
	


func _exit_tree():
	# Clean-up of the plugin goes here.
	# Always remember to remove it from the engine when deactivated.
	remove_custom_types()



func load_custom_types():
	var dir = DirAccess.open(PATH)
	
	if not dir:
		push_error("Nije moguće otvoriti %s folder." % PATH)
		return
	
	dir.list_dir_begin() # TODOGODOT4 fill missing arguments https://github.com/godotengine/godot/pull/40547
	var element_name = dir.get_next()
	while element_name != "":
		
		if dir.current_is_dir():
			var script_path = "%s/%s/%s.cs" % [PATH, element_name, element_name]
			if dir.file_exists(script_path):
				var script: Script = load(script_path)
				var base_type = script.get_instance_base_type()
				
				add_custom_type(element_name, base_type, script, default_icon)
				loaded_types.append(element_name)
				print("Učitan custom node: %s" % element_name)
		
		element_name = dir.get_next()


func remove_custom_types():
	for node_name in loaded_types:
		remove_custom_type(node_name)


