import sys
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import WebDriverException, TimeoutException

def get_driver():
    print("get_driver")
    service = Service('/usr/bin/chromedriver')
    options = Options()
    options.add_argument("--headless")
    options.add_argument("--no-sandbox")
    options.add_argument("--disable-dev-shm-usage")
    options.add_argument("window-size=1200,800")
    options.add_argument("--remote-debugging-port=9222")
    driver = webdriver.Chrome(service=service, options=options)
    print("get_driver finish")
    return driver

def find_element_coordinates(identifier, identifier_type, index=0):
    driver = get_driver()
    max_retries = 3
    for attempt in range(max_retries):
        try:
            print(f"Attempt {attempt + 1} to find element with {identifier_type}: {identifier}, index: {index}")
            if identifier_type == "class":
                elements = WebDriverWait(driver, 5).until(
                    EC.presence_of_all_elements_located((By.CLASS_NAME, identifier))
                )
            elif identifier_type == "src":
                elements = WebDriverWait(driver, 5).until(
                    EC.presence_of_all_elements_located((By.XPATH, f"//img[contains(@src, '{identifier}')]"))
                )
            elif identifier_type == "identifier":
                elements = WebDriverWait(driver, 5).until(
                    EC.presence_of_all_elements_located((By.XPATH, f"//*[contains(@identifier, '{identifier}')]"))
                )
            elif identifier_type == "title":
                elements = WebDriverWait(driver, 5).until(
                    EC.presence_of_all_elements_located((By.XPATH, f"//*[contains(@title, '{identifier}')]"))
                )
            elif identifier_type == "type":
                elements = WebDriverWait(driver, 5).until(
                    EC.presence_of_all_elements_located((By.XPATH, f"//*[@type='{identifier}']"))
                )
            elif identifier_type == "tag":
                elements = WebDriverWait(driver, 5).until(
                    EC.presence_of_all_elements_located((By.TAG_NAME, identifier))
                )
            elif identifier_type == "buttontext":
                buttons = WebDriverWait(driver, 5).until(
                    EC.presence_of_all_elements_located((By.XPATH, "//button[not(ancestor::*[contains(@style,'display:none') or contains(@style,'display: none')])]"))
                )
                elements = [button for button in buttons if identifier.lower() in button.text.lower()]
                if not elements:
                    raise TimeoutException(f"No buttons found with text: {identifier}")
            elif identifier_type == "labeltext":
                labels = WebDriverWait(driver, 5).until(
                    EC.presence_of_all_elements_located((By.XPATH, "//label[not(ancestor::*[contains(@style,'display:none') or contains(@style,'display: none')])]"))
                )
                elements = [label for label in labels if identifier.lower() in label.text.lower()]
                if not elements:
                    raise TimeoutException(f"No labels found with text: {identifier}")
            else:
                raise ValueError("Invalid identifier_type. Use 'class', 'src', 'identifier', 'title', 'type', 'tag', 'buttontext' or 'labeltext'.")
            
            if not elements:
                raise TimeoutException(f"No elements found with {identifier_type}: {identifier}")
            
            if index >= len(elements):
                raise IndexError(f"Index {index} is out of range. Only {len(elements)} elements found.")
            
            element = elements[index]
            location = element.location
            size = element.size
            center_x = int(location['x'] + size['width'] / 2)
            center_y = int(location['y'] + size['height'] / 2)
            print(f"Element {element} found. Center at: ({center_x}, {center_y})")
            return center_x, center_y
        except TimeoutException:
            print(f"Timeout on attempt {attempt + 1}")
            if attempt == max_retries - 1:
                raise

if __name__ == "__main__":
    print("Called test_script.py")
    print(f"Script called with arguments: {sys.argv}")
    if len(sys.argv) < 2:
        print("Usage: python test_script.py [args...]")
        print("Commands: find")
        sys.exit(1)

    command = sys.argv[1]
    print(f"Command: {command}")

    if command == "find":
        if len(sys.argv) < 4:
            print("Usage: python find_element.py find <identifier_type> <identifier> [index]")
            sys.exit(1)
        
        identifier_type = sys.argv[2]
        identifier = sys.argv[3]
        index = int(sys.argv[4]) if len(sys.argv) > 4 else 0
        
        print(f"Finding element: type={identifier_type}, identifier={identifier}, index={index}")
        try:
            x, y = find_element_coordinates(identifier, identifier_type, index)
            x += 360
            y += 140 + 26
            print(f"{x},{y}", flush=True)
        except Exception as e:
            print(f"An error occurred: {str(e)}")
            sys.exit(1)