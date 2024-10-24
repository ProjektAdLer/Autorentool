import time
import docker

client = docker.from_env()

def check_container_log(container_name, success_pattern):
    max_attempts = 3
    for attempt in range(max_attempts):
        try:
            container = client.containers.get(container_name)
            logs = container.logs(tail=100).decode("utf-8")
            return success_pattern in logs
        except docker.errors.NotFound:
            return None
        time.sleep(2)  # Wait before retrying
    print(f"Failed to fetch logs for {container_name} after {max_attempts} attempts")
    return False


containers = {
    "adlertestenvironment-backend-1": "Hosting started",
    "adlertestenvironment-moodle-1": "finished adler setup/update script",
    "adlertestenvironment-frontend-1": "Configuration complete; ready for start up",
    "adlertestenvironment-db_backend-1": "ready for connections.",
    "adlertestenvironment-phpmyadmin-1": "resuming normal operations",
    "adlertestenvironment-db_moodle-1": "ready for connections."
}

start_time = time.time()
timeout = 180  # 3 minutes timeout
ready_containers = set()

while len(ready_containers) < len(containers):
    print(f"\nÜberprüfe Container-Status (Vergangene Zeit: {int(time.time() - start_time)} Sekunden):")
    for container, pattern in containers.items():
        if container in ready_containers:
            print(f"  - {container}: Bereit")
        else:
            result = check_container_log(container, pattern)
            if result is None:
                print(f"  - {container}: Noch nicht gestartet")
            elif result:
                print(f"  - {container}: Gerade bereit geworden")
                ready_containers.add(container)
            else:
                print(f"  - {container}: Noch nicht bereit")

    if len(ready_containers) == len(containers):
        print("\nAlle Container sind bereit!")
        break

    if time.time() - start_time > timeout:
        print(f"\nTimeout erreicht. Nicht alle Container sind bereit geworden.")
        for container in containers:
            if container not in ready_containers:
                print(f"  - {container}: Nicht bereit")
        exit(0)

    time.sleep(10)  # Warte 10 Sekunden vor der nächsten Überprüfung

print(f"\nAlle Container wurden erfolgreich gestartet. Gesamtzeit: {int(time.time() - start_time)} Sekunden")
