compile:
	javac -d bin --source-path src src/*.java

run: compile
	java -cp bin Main