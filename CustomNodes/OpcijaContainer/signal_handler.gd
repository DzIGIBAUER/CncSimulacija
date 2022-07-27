extends Node

onready var opcija_container = get_parent()

var opcija_event = preload("res://CustomNodes/OpcijaContainer/OpcijaEvent.cs")


## Trebala mi je metoda koja uzima promenljiv broj argumenata, promenljivih tipova,
# C# metoda sa (params object[] parametri) uporno daje prazne objekte,
# pa sam morao ovako.
# Uzima do 7 arg gde u zavisnosti od argumenata eventa, vecina moze da bude null.
# Kreiramo OpcijaEvent objekat koji inherit-uje Godot Resource->Reference->Object
# jer uz signal mogu da budu prosledjeni samo Godot Object ili objekat koji ga inherituje.
func on_interaktivna_kontrola_value_change(p1=null,p2=null,p3=null,p4=null,p5=null,p6=null,p7=null):
	var parametri = Array([p1,p2,p3,p4,p5,p6,p7])
	var non_null = Array() # argumenti koji nisu null
	
	for p in parametri:
		if p:
			non_null.append(p)
	
	var event = opcija_event.new(opcija_container.name, non_null[opcija_container.SignalParametar])
	opcija_container.emit_signal("OpcijaPromenjena", event)


